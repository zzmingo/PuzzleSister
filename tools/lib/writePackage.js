const path = require('path')
const fs = require('fs')
const crypto = require('crypto')
const fse = require('fs-extra')

const encryptKey = "7ydsdgbp"

module.exports = function(packageItem, dist) {
  let packagePath = path.resolve(dist, packageItem.id)
  fse.ensureDirSync(packagePath)
  fse.writeFileSync(path.resolve(packagePath, 'Package.csv'), encrypt(packageItem.Package, encryptKey), 'utf8')
  fse.writeFileSync(path.resolve(packagePath, 'Question.csv'), encrypt(packageItem.Question, encryptKey), 'utf8')
}

function encrypt (message, key) {
  key = key.length >= 8 ? key.slice(0, 8) : key.concat('0'.repeat(8 - key.length))
  const keyHex = Buffer.from(key)
  const cipher = crypto.createCipheriv('des-cbc', keyHex, keyHex)
  let result = cipher.update(message, 'utf8', 'base64')
  result += cipher.final('base64');
  return result
}