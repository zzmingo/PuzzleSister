using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace PuzzleSister {

	public class PackageTest {

		[Test]
		public void TestLoadPackage() {
			Package pkg = new Package();
			pkg.id = "Test0001";
			pkg.name = "测试包";
			pkg.path = "Packages/Test/Question";
			pkg.type = Package.Type.CSV;
			pkg.source = Package.Source.Resources;
			var questionList = pkg.Load();
			Assert.AreEqual(2, questionList.Count);
		}

		[Test]
		public void TestRepository() {
			Repository.shared.LoadPackages();
			var questionList = Repository.shared.GetPackageById("PKG0001").Load();
			Assert.AreEqual(2, questionList.Count);
		}
	}

}
