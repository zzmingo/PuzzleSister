const path = require('path')
const fs = require('fs')
const crypto = require('crypto')
const fse = require('fs-extra')
const XLSX = require('js-xlsx')

const encryptKey = "7ydsdgbp"

module.exports = function(xlsxFile) {
  const workbook = XLSX.readFile(xlsxFile)
  const dlcSheet = workbook.Sheets["DLC"]
  const questionSheet = workbook.Sheets["Question"]
  if (!dlcSheet || !questionSheet) {
    throw new Error("Incompleted workbook")
  }

  // handle DLC
  let dlcId
  let dlcArray = XLSX.utils.sheet_to_json(dlcSheet, {
    blankrows: false
  });
  dlcArray.shift();
  dlcId = dlcArray[0].id
  dlcArray = dlcArray.filter(item => !!item.id).map((item) => {
    delete item[""]
    return item
  });
  let dlcCSV = array2csv(dlcArray)

  // handle questions
  let questionArray = XLSX.utils.sheet_to_json(questionSheet, {
    blankrows: false
  })
  questionArray.shift()
  questionArray = questionArray.filter(isValidQuestion).map((item) => {
    delete item[""]
    return item
  })
  let questionCSV = array2csv(questionArray)
  return {
    id: dlcId,
    Package: dlcCSV,
    Question: questionCSV
  }
}

function isValidQuestion(item) {
  return item.id && item.title && item.A && item.B && item.C && item.D && item.result && item.explain
}

function array2csv(array) {
  const keys = Object.keys(array[0])
  const lines = [keys.join(',')]
  array.forEach(item => {
    const values = []
    keys.forEach(key => {
      values.push(escape(item[key] || ''))
    })
    lines.push(values.join(','))
  })
  return lines.join('\n')
}

function escape(value) {
  value = value.trim().replace(/\n/g, "。").replace(/\r/g, "").replace(/\n\r/g, "。")
  return value;
}