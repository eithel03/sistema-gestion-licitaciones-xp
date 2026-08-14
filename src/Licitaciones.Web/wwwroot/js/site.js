// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function ($) {
  if (!$ || !$.validator) {
    return;
  }

  var proveedorNameRegex;
  try {
    proveedorNameRegex = new RegExp("^[\\p{L}\\p{N} .,()]+$", "u");
  } catch (_) {
    proveedorNameRegex = /^[A-Za-z0-9 .,()]+$/;
  }

  $.validator.addMethod("proveedornombre", function (value, element) {
    if (this.optional(element)) {
      return true;
    }

    return proveedorNameRegex.test(value);
  });

  $.validator.unobtrusive.adapters.addBool("proveedornombre");

  $.validator.methods.number = function (value, element) {
    if (this.optional(element)) {
      return true;
    }

    var normalized = value.replace(",", ".");
    return normalized.indexOf(".") === normalized.lastIndexOf(".")
      && /^-?(?:\d+|\d+\.\d+)$/.test(normalized);
  };
})(window.jQuery);
