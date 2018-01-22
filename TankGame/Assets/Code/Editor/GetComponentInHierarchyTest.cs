using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace TankGame.Testing
{

    public class GetComponentInHierarchyTest
    {
        private GameObject _parent, _child, _grandChild;

        private GetComponentInHierarchyTester SetUp(bool includeInactive, bool componentInParent, bool setActive)
        {
            _parent = new GameObject();
            _child = new GameObject();
            _grandChild = new GameObject();

            _grandChild.transform.parent = _child.transform;
            _child.transform.parent = _parent.transform;

            GetComponentInHierarchyTester tester = _child.AddComponent<GetComponentInHierarchyTester>();
            tester.Setup(includeInactive, componentInParent, setActive);

            return tester;

        }
        [Test]
        public void ComponentInChild()
        {
            System.IO.TextWriter writer = TestContext.Out;

            GetComponentInHierarchyTester tester = SetUp(true, false, true);
            TestComponent result = tester.Run();
            Assert.NotNull(result);
            writer.WriteLine("Include Inactive, Set Active");

            tester.Setup(true, false, true);
            Assert.NotNull(tester.Run());

            tester.Setup(false, false, true);
            Assert.NotNull(tester.Run());

            tester.Setup(false, false, false);
            Assert.Null(tester.Run());

            UnityEditor.
        }

        [Test]
        public void ComponentInParent()
        {
            GetComponentInHierarchyTester tester = SetUp(true, true, true);
            TestComponent result = tester.Run();
            Assert.NotNull(result);

            tester.Setup(true, true, true);
            Assert.NotNull(tester.Run());

            tester.Setup(false, true, true);
            Assert.NotNull(tester.Run());

            tester.Setup(false, true, false);
            Assert.Null(tester.Run());
        }

 

    }
}
