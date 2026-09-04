// Checklisty zadań zapamiętywane w localStorage (per lekcja).
(function () {
  document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".check input[type=checkbox]").forEach(function (cb, i) {
      var key = "check:" + location.pathname + ":" + (cb.id || i);
      try { cb.checked = localStorage.getItem(key) === "1"; } catch (e) {}
      cb.addEventListener("change", function () {
        try { localStorage.setItem(key, cb.checked ? "1" : "0"); } catch (e) {}
      });
    });
  });
})();
