const path = require('path')
const glob = require('glob')
const fse = require('fs-extra')
const xlsx2package = require('./xlsx2package')
const fakePackage = require('./fakePackage')
const writePackage = require('./writePackage')

fse.emptyDirSync(path.resolve(__dirname, '../generated/builtin'))
fse.emptyDirSync(path.resolve(__dirname, '../generated/dlc'))

const pkg = fakePackage("FAKE001", "自动生成的")
writePackage(pkg, path.resolve(__dirname, '../generated/builtin'))

const builtins = glob.sync(path.resolve(__dirname, '../../xlsx/builtin/*.xlsx'))
builtins.forEach(xlsxFile => {
  if (xlsxFile.startsWith('~$')) return
  const pkg = xlsx2package(xlsxFile)
  writePackage(pkg, path.resolve(__dirname, '../generated/builtin'))
})

const dlcs = glob.sync(path.resolve(__dirname, '../../xlsx/dlc/*.xlsx'))
dlcs.forEach(xlsxFile => {
  if (path.basename(xlsxFile).startsWith('~$')) return
  const pkg = xlsx2package(xlsxFile)
  writePackage(pkg, path.resolve(__dirname, '../generated/dlc'))
})