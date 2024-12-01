document.addEventListener("DOMContentLoaded", function() {
  const form = document.getElementById("surveyForm");
  const wrapper = document.getElementById("wrapper");

  if (localStorage.getItem("surveySubmitted") === "true") {
    wrapper.innerHTML = `
        <h1>Дякуємо за вашу участь!</h1>
        <p>Вашу відповідь було успішно надіслано.</p>
    `;
    return;
}

  form.addEventListener("submit", function(event) {
      event.preventDefault();

      const formData = new FormData(form);
      let responses = {};
      formData.forEach((value, key) => {
          responses[key] = value;
      });

      console.log("Survey Responses:", responses);

      fetch('/submit-survey', {
          method: 'POST',
          headers: {
              'Content-Type': 'application/json',
          },
          body: JSON.stringify(responses),
      })
      .then(response => response.json())
      .then(data => {
          if (data.success) {
            localStorage.setItem("surveySubmitted", "true");
            wrapper.innerHTML = `
                <h1>Дякуємо за вашу участь!</h1>
                <p>Вашу відповідь було успішно надіслано.</p>
            `;
          } else {
              alert("Виникла помилка, повторіть спробу!");
          }
      })
      .catch(error => {
          console.error("Error submitting survey:", error);
          alert("Виникла помилка, повторіть спробу!");
      });
  });
});