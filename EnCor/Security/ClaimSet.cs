using System.Collections.Generic;

namespace EnCor.Security
{
    public class ClaimSet : IEnumerable<ClaimObject>
    {
        private List<ClaimObject> _Claims = new List<ClaimObject>();

        internal ClaimSet(IEnumerable<ClaimObject> claims)
        {
            foreach (ClaimObject claim in claims)
            {
                _Claims.Add(claim);
            }
        }

        internal IEnumerable<ClaimObject> FindClaims(string claimType, string right)
        {
            return _Claims.FindAll(delegate(ClaimObject c)
            {
                return c.ClaimType == claimType && c.Right == right;
            });
        }

        #region IEnumerable<ClaimObject> Members

        public IEnumerator<ClaimObject> GetEnumerator()
        {
            return _Claims.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _Claims.GetEnumerator();
        }

        #endregion
    }
}
