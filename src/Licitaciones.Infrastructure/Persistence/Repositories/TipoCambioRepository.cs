using Licitaciones.Application.TiposCambio;
using Licitaciones.Domain.TiposCambio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;

namespace Licitaciones.Infrastructure.Persistence.Repositories;

public sealed class TipoCambioRepository : ITipoCambioRepository
{
    private readonly LicitacionesDbContext _context;
    private IDbContextTransaction? _activationTransaction;

    public TipoCambioRepository(LicitacionesDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TipoCambio tipoCambio, CancellationToken cancellationToken = default) =>
        await Set.AddAsync(tipoCambio, cancellationToken);

    public Task<TipoCambio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Set.SingleOrDefaultAsync(tipoCambio => tipoCambio.Id == id, cancellationToken);

    public Task<TipoCambio?> GetActiveAsync(CancellationToken cancellationToken = default) =>
        Set.AsNoTracking().SingleOrDefaultAsync(tipoCambio => tipoCambio.Activo, cancellationToken);

    public async Task<TipoCambioPage> ListAsync(TipoCambioQuery query, CancellationToken cancellationToken = default)
    {
        var tiposCambio = Set.AsNoTracking()
            .OrderByDescending(tipoCambio => tipoCambio.Activo)
            .ThenByDescending(tipoCambio => tipoCambio.Fecha)
            .ThenByDescending(tipoCambio => tipoCambio.CreatedAt)
            .ThenBy(tipoCambio => tipoCambio.Id);
        var total = await tiposCambio.CountAsync(cancellationToken);
        var items = await tiposCambio.Skip((query.ValidPage - 1) * query.ValidPageSize).Take(query.ValidPageSize).ToListAsync(cancellationToken);
        return new TipoCambioPage(items.Select(TipoCambioResponse.FromDomain).ToList(), total, query.ValidPage, query.ValidPageSize);
    }

    public async Task DeactivateAllExceptAsync(Guid activeId, DateTimeOffset updatedAt, CancellationToken cancellationToken = default)
    {
        if (_context.Database.CurrentTransaction is null)
        {
            _activationTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        try
        {
            await Set
                .Where(tipoCambio => tipoCambio.Id != activeId && tipoCambio.Activo)
                .ExecuteUpdateAsync(updates => updates
                    .SetProperty(tipoCambio => tipoCambio.Activo, false)
                    .SetProperty(tipoCambio => tipoCambio.UpdatedAt, updatedAt),
                    cancellationToken);
        }
        catch
        {
            await CompleteActivationTransactionAsync(commit: false, cancellationToken);
            throw;
        }
    }

    public void Remove(TipoCambio tipoCambio) => Set.Remove(tipoCambio);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await CompleteActivationTransactionAsync(commit: true, cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            await CompleteActivationTransactionAsync(commit: false, cancellationToken);
            throw new TipoCambioConcurrencyException();
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException postgres &&
            postgres.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            await CompleteActivationTransactionAsync(commit: false, cancellationToken);
            throw new TipoCambioActiveConflictException();
        }
        catch
        {
            await CompleteActivationTransactionAsync(commit: false, cancellationToken);
            throw;
        }
    }

    private async Task CompleteActivationTransactionAsync(bool commit, CancellationToken cancellationToken)
    {
        if (_activationTransaction is null)
        {
            return;
        }

        try
        {
            if (commit)
            {
                await _activationTransaction.CommitAsync(cancellationToken);
            }
            else
            {
                await _activationTransaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            await _activationTransaction.DisposeAsync();
            _activationTransaction = null;
        }
    }

    private DbSet<TipoCambio> Set => _context.Set<TipoCambio>();
}
