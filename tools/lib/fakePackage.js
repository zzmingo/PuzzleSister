const faker = require('faker')
const shuffle = require('shuffle-array')

module.exports = function(id, name) {

  // Package
  let lines = ["id,name,order,thumb,description"]
  lines.push([id, name, 0, "", "自动生成的题库"].join(','))
  const packageCSVStr = lines.join('\n')
  const pkgId = lines[1][0]
  
  // Question
  lines = ["id,title,A,B,C,D,result,explain"]
  for(let i=0; i<100; i++) {
    lines.push(makeQuestion())
  }

  return {
    id: id,
    Package: packageCSVStr,
    Question: lines.join('\n')
  }
}

const factories = [
  makeFactory("city", [
    faker.address.city(),
    faker.address.country(),
    faker.address.state(),
    faker.address.countryCode(),
  ]),
  makeFactory("country", [
    faker.address.country(),
    faker.address.city(),
    faker.address.state(),
    faker.address.countryCode(),
  ]),
  makeFactory("country code", [
    faker.address.countryCode(),
    faker.address.country(),
    faker.address.city(),
    faker.address.state(),
  ]),
  makeFactory("state", [
    faker.address.state(),
    faker.address.countryCode(),
    faker.address.country(),
    faker.address.city(),
  ])
]

function makeFactory(kind, options) {
  return function() {
    const result = options[0]
    shuffle(options)
    return [
      "FAKE" + Date.now() + Math.random(),
      "Which is a " + kind + "?"
    ].concat(options).concat([
      getResultOption(result, options),
      result + " is a " + kind + " and the others is not"
    ])
  }
}

function getResultOption(result, options) {
  switch(options.indexOf(result)) {
    case 0: return "A"
    case 1: return "B"
    case 2: return "C"
    case 3: return "D"
  }
}

function makeQuestion() {
  return factories[Math.floor(Math.random()*factories.length)]().join(',')
}