using System;

namespace NHibernate.Test.NHSpecificTest.NH3985
{
	public partial class Process
	{
		#region Extensibility Method Definitions

		public override bool Equals(object obj)
		{
			Process toCompare = obj as Process;
			if (toCompare == null)
			{
				return false;
			}

			if (Object.Equals(this.ProcessID, default(long)) &&
				Object.Equals(toCompare.ProcessID, default(long)))
				return ReferenceEquals(this, toCompare);

			if (!Object.Equals(this.ProcessID, toCompare.ProcessID))
				return false;

			return true;
		}

		public override int GetHashCode()
		{
			int hashCode = 13;

			// on transient objects, use the basic GetHashCode()
			if (Object.Equals(this.ProcessID, default(long)))
				return base.GetHashCode();

			hashCode = (hashCode * 7) + ProcessID.GetHashCode();
			return hashCode;
		}

		#endregion

		public virtual int ProcessID { get; set; }

		public virtual string Name { get; set; }
	}
}
