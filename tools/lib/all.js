const path = require('path')
const glob = require('glob')
const xlsx2package = require('./xlsx2package')

const builtins = glob.sync(path.resolve(__dirname, '../xlsx/builtin/*.xlsx'))
builtins.forEach(xlsxFile => {
  xlsx2package(xlsxFile, path.resolve(__dirname, '../generated/builtin'))
})

const dlcs = glob.sync(path.resolve(__dirname, '../xlsx/dlc/*.xlsx'))
dlcs.forEach(xlsxFile => {
  xlsx2package(xlsxFile, path.resolve(__dirname, '../generated/dlc'))
})