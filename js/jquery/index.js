// Example of using API4AI OCR text recognition.

// Use 'demo' mode just to try api4ai for free. Free demo is rate limited.
// For more details visit:
//   https://api4.ai

// Use 'rapidapi' if you want to try api4ai via RapidAPI marketplace.
// For more details visit:
//   https://rapidapi.com/api4ai-api4ai-default/api/ocr43/details
const MODE = 'demo'

// Your RapidAPI key. Fill this variable with the proper value if you want
// to try api4ai via RapidAPI marketplace.
const RAPIDAPI_KEY = ''

const OPTIONS = {
  demo: {
    url: 'https://demo.api4ai.cloud/ocr/v1/results',
    headers: { 'A4A-CLIENT-APP-ID': 'sample' }
  },
  rapidapi: {
    url: 'https://ocr43.p.rapidapi.com/v1/results',
    headers: { 'X-RapidAPI-Key': RAPIDAPI_KEY }
  }
}

document.addEventListener('DOMContentLoaded', function (event) {
  const input = document.getElementById('file')
  const raw = document.getElementById('raw')
  const sectionRaw = document.getElementById('sectionRaw')
  const parsed = document.getElementById('parsed')
  const sectionParsed = document.getElementById('sectionParsed')
  const spinner = document.getElementById('spinner')

  input.addEventListener('change', (event) => {
    const file = event.target.files[0]
    if (!file) {
      return false
    }

    sectionRaw.hidden = true
    sectionParsed.hidden = true
    spinner.hidden = false

    // Preapare request: form.
    const form = new FormData()
    form.append('image', file)

    // Make request.
    // eslint-disable-next-line  no-undef -- $ (jquery) appended to the html file via cdn.
    $.ajax({
      type: 'POST',
      url: OPTIONS[MODE].url,
      data: form,
      headers: OPTIONS[MODE].headers,
      processData: false,
      contentType: false
    })
      .done(function (response) {
        // Print raw response.
        raw.textContent = JSON.stringify(response, undefined, 2)
        sectionRaw.hidden = false
        // Parse response and display recognized text.
        let textContent = ''
        for (const i in response.results) {
          const result = response.results[i]
          const text = result.entities[0].objects[0].entities[0].text
          textContent += text + '\n'
        }
        parsed.textContent = textContent
        sectionParsed.hidden = false
      })
      .fail(function (error) {
        // Error can be handled here.
        console.error(error)
      })
      .always(function () {
        spinner.hidden = true
      })
  })
})
