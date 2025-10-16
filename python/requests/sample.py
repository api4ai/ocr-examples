#!/usr/bin/env python3

"""Example of using API4AI OCR text recognition."""

import os
import sys

import requests


# Use 'demo' mode just to try api4ai for free. ⚠️ Free demo is rate limited and must not be used in real projects.
#
# Use 'normal' mode if you have an API Key from the API4AI Developer Portal. This is the method that users should normally prefer.
#
# Use 'rapidapi' if you want to try api4ai via RapidAPI marketplace.
# For more details visit:
#   https://rapidapi.com/api4ai-api4ai-default/api/ocr43/details
MODE = 'demo'

# Your API4AI key. Fill this variable with the proper value if you have one.
API4AI_KEY = ''

# Your RapidAPI key. Fill this variable with the proper value if you want
# to try api4ai via RapidAPI marketplace.
RAPIDAPI_KEY = ''


OPTIONS = {
    'demo': {
        'url': 'https://demo.api4ai.cloud/ocr/v1/results'
    },
    'normal': {
        'url': 'https://api4ai.cloud/ocr/v1/results',
        'headers': {'X-API-KEY': API4AI_KEY}
    },
    'rapidapi': {
        'url': 'https://ocr43.p.rapidapi.com/v1/results',
        'headers': {'X-RapidAPI-Key': RAPIDAPI_KEY}
    }
}


if __name__ == '__main__':
    # Parse args.
    image = sys.argv[1] if len(sys.argv) > 1 else 'https://static.api4.ai/samples/ocr-1.png'

    if '://' in image:
        # POST image via URL.
        response = requests.post(
            OPTIONS[MODE]['url'],
            headers=OPTIONS[MODE].get('headers'),
            data={'url': image})
    else:
        # POST image as file.
        with open(image, 'rb') as image_file:
            response = requests.post(
                OPTIONS[MODE]['url'],
                headers=OPTIONS[MODE].get('headers'),
                files={'image': (os.path.basename(image), image_file)}
            )

    # Print raw response.
    print(f'💬 Raw response:\n{response.text}\n')

    # Parse response and print recognized text.
    results = response.json()['results']
    for result in results:
        text = result['entities'][0]['objects'][0]['entities'][0]['text']
        page_tip = ''
        if 'page' in result:
            page = result['page']
            page_tip = f' on page {page}'
        print(f'💬 Recognized text{page_tip}:\n{text}\n')
