﻿using NUnit.Framework;
using System;

namespace NHibernate.Test.NHSpecificTest.NH3985
{
	/// <summary>
	/// The test verifies that subsequent child sessions are not issued in already-disposed state.
	/// </summary>
	[TestFixture]
	public class Fixture : BugTestCase
	{
		[Test]
		public void GetChildSession_ShouldReturnNonDisposedInstance()
		{
			using (var rootSession = OpenSession())
			{
				using (var childSession1 = rootSession.GetChildSession())
				{
				}

				using (var childSession2 = rootSession.GetChildSession())
				{
					Assert.DoesNotThrow(new TestDelegate(() => { childSession2.Get<Process>(Guid.NewGuid()); }));
				}
			}
		}
	}
}
