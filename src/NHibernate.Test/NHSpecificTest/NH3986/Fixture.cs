using NUnit.Framework;
using System.Collections;

namespace NHibernate.Test.NHSpecificTest.NH3986
{
	/// <summary>
	/// Verifies that both serial and nested child sessions have unique instances.
	/// </summary>
	[TestFixture]
	public class Fixture : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[0]; }
		}

		[Test]
		public void MultipleChildSessionInstances_ShouldBeUnique()
		{
			using (var rootSession = OpenSession())
			{
				ISession childSession1, childSession2, childSession3;
				using (childSession1 = rootSession.GetChildSession())
				{
					Assert.IsNotNull(childSession1);
					Assert.IsFalse(object.ReferenceEquals(childSession1, rootSession));
				}

				// test serial child sessions
				using (childSession2 = rootSession.GetChildSession())
				{
					Assert.IsNotNull(childSession2);
					Assert.IsFalse(object.ReferenceEquals(childSession2, rootSession));
					Assert.IsFalse(object.ReferenceEquals(childSession2, childSession1));

					// test nested child sessions
					using (childSession3 = childSession2.GetChildSession())
					{
						Assert.IsNotNull(childSession3);
						Assert.IsFalse(object.ReferenceEquals(childSession3, rootSession));
						Assert.IsFalse(object.ReferenceEquals(childSession3, childSession1));
						Assert.IsFalse(object.ReferenceEquals(childSession3, childSession2));
					}
				}
			}
		}

		[Test]
		public void ClosingRootSession_ShouldCloseChildSessions()
		{
			using (var rootSession = OpenSession())
			{
				using (var childSession1 = rootSession.GetChildSession())
				{
					Assert.IsTrue(childSession1.IsOpen);

					using (var childSession2 = rootSession.GetChildSession())
					{
						Assert.IsTrue(childSession2.IsOpen);

						// this shouldn't typically occur within a nested child scope wrapped in a "using" clause;
						// testing the edge case when child sessions are managed without "using"
						rootSession.Close();

						Assert.IsFalse(childSession2.IsOpen);
						Assert.IsFalse(childSession1.IsOpen);
					}
				}
			}
		}

		[Test]
		public void ClosingChildSession_ShouldNotCloseRootSession()
		{
			ISession rootSession;
			using (rootSession = OpenSession())
			{
				using (var childSession1 = rootSession.GetChildSession())
				{
					using (var childSession2 = rootSession.GetChildSession())
					{
					}
					Assert.IsTrue(rootSession.IsOpen);
				}
				Assert.IsTrue(rootSession.IsOpen);
			}
			Assert.IsFalse(rootSession.IsOpen);
		}
	}
}
