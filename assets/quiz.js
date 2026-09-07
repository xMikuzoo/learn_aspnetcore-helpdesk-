// Kurs: ASP.NET Core dla frontendowca — quiz z natychmiastowym feedbackiem.
// Użycie w HTML:
// <div class="quiz" data-answer="1">
//   <div class="q">Pytanie?</div>
//   <button class="opt">Opcja A</button>
//   <button class="opt">Opcja B</button>
//   <div class="why">Wyjaśnienie widoczne po odpowiedzi.</div>
// </div>
// data-answer = indeks (od 0) poprawnej opcji. Wszystkie opcje powinny mieć tę samą
// długość, żeby formatowanie nie zdradzało odpowiedzi.
(function () {
  function init(quiz, index) {
    var answer = parseInt(quiz.getAttribute("data-answer"), 10);
    if (isNaN(answer)) {
      console.error("Quiz bez data-answer — każda odpowiedź wyjdzie zła:", quiz);
      return;
    }
    var opts = Array.prototype.slice.call(quiz.querySelectorAll("button.opt"));
    var key = "quiz:" + location.pathname + ":" + (quiz.id || index);
    var attempts = 0;

    function reveal(chosen) {
      opts.forEach(function (b, i) {
        b.disabled = true;
        if (i === answer) b.classList.add("correct");
        else if (i === chosen) b.classList.add("wrong");
      });
      quiz.classList.add("answered");
    }

    opts.forEach(function (btn, i) {
      btn.addEventListener("click", function () {
        attempts++;
        if (i === answer) {
          reveal(i);
          try { localStorage.setItem(key, JSON.stringify({ ok: true, attempts: attempts, at: Date.now() })); } catch (e) {}
        } else {
          // Zła odpowiedź: zaznacz, pozwól spróbować ponownie (retrieval practice > pokazanie odpowiedzi).
          btn.classList.add("wrong");
          btn.disabled = true;
          try { localStorage.setItem(key, JSON.stringify({ ok: false, attempts: attempts, at: Date.now() })); } catch (e) {}
        }
      });
    });
  }
  document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".quiz").forEach(init);
  });
})();
