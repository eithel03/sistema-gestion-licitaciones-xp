# Vision y alcance

## Vision

El proyecto busca construir una aplicacion web modular para gestionar licitaciones, proveedores, ofertas, niveles de aprobacion y conversion visual CRC/USD mediante Extreme Programming. La planificacion se organiza con historias de usuario, iteraciones uniformes, pequenas liberaciones, TDD, programacion en parejas, integracion continua y trazabilidad documental.

Este documento puede actualizarse a medida que se obtenga retroalimentacion del cliente.

## Problema

El sistema responde a la necesidad de centralizar informacion y evidencia sobre:

- Licitaciones.
- Proveedores.
- Ofertas.
- Mejor oferta.
- Aprobaciones.
- Conversion monetaria.
- Evidencias tecnicas y metodologicas.

Sin una fuente organizada, el seguimiento de reglas, decisiones, pruebas, entregas y documentacion queda disperso y dificulta la defensa del proyecto.

## Usuarios principales

- Usuario administrativo.
- Consumidor de API.
- Equipo de desarrollo.
- Persona docente o evaluadora.

## Alcance incluido

- Landing page.
- Navegacion.
- CRUD de licitaciones.
- CRUD de proveedores.
- CRUD de ofertas.
- CRUD de niveles de aprobacion.
- CRUD de tipos de cambio.
- Estados de licitacion.
- Validaciones.
- Mejor oferta.
- Clasificacion.
- Aprobador.
- CRC/USD.
- API REST.
- PostgreSQL.
- Pruebas.
- Docker.
- Kubernetes.
- GitHub Actions.
- Documentacion en `/docs`.

## Fuera de alcance inicial

- Integracion obligatoria con servicios externos de tipo de cambio.
- Aplicaciones moviles nativas.
- Microservicios sin justificacion.
- Funcionalidades no solicitadas por las historias vigentes.

## Restricciones

- .NET 9.
- ASP.NET Core MVC.
- ASP.NET Core Web API.
- Entity Framework Core 9.
- PostgreSQL 16 o superior.
- CRC como fuente de verdad.
- Extreme Programming como unica metodologia.
- Documentacion unicamente en `/docs`.
- Trabajo en pareja con rotacion de driver y navigator.
- Pruebas y TDD.
- Docker, Kubernetes y GitHub Actions.

## Criterios generales de exito

- Aplicacion funcional.
- Reglas de negocio verificables.
- Pruebas automatizadas.
- Pequenas liberaciones.
- Integracion continua satisfactoria.
- Despliegue reproducible.
- Documentacion y trazabilidad completas.
- Ambos integrantes pueden defender el sistema.
