#!/bin/bash

######################################################
# NOTE:                                              #
#   This script requires "jq" command line tool!     #
#   See https://stedolan.github.io/jq/               #
######################################################


IMAGE=${1}
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Run base sample script to get raw output.
raw_response=$(bash ${DIR}/sample.sh "${IMAGE}")
echo "💬 Raw response:"
echo "${raw_response}"
echo ""

# Parse response and print recognized text.
echo "💬 Recognized text:"
jq -r ".results[0].entities[0].objects[0].entities[0].text" <<< ${raw_response}
