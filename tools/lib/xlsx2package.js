const path = require('path')
const fs = require('fs')
const fse = require('fs-extra')
const XLSX = require('js-xlsx')

function xlsx2package(xlsxFile, dist) {
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
  })
  dlcArray.shift()
  dlcId = dlcArray[0].id
  dlcArray = dlcArray.filter(item => !!item.id).map((item) => {
    delete item[""]
    return item
  })
  let dlcCSV = array2csv(dlcArray)
  let dlcPath = path.resolve(dist, dlcId)
  fse.ensureDirSync(dlcPath)
  fse.writeFileSync(path.resolve(dlcPath, 'Package.csv'), dlcCSV, 'utf8')

  // handle questions
  let questionArray = XLSX.utils.sheet_to_json(questionSheet, {
    blankrows: false
  })
  questionArray.shift()
  questionArray = questionArray.filter(item => !!item.id).map((item) => {
    delete item[""]
    return item
  })
  let questionCSV = array2csv(questionArray)
  fse.writeFileSync(path.resolve(dlcPath, 'Question.csv'), questionCSV, 'utf8')
}

function array2csv(array) {
  const keys = Object.keys(array[0])
  const lines = [keys.join(',')]
  array.forEach(item => {
    const values = []
    keys.forEach(key => {
      values.push(item[key] || '')
    })
    lines.push(values.join(','))
  })
  return lines.join('\n')
}

module.exports = xlsx2package