using System;
using System.Data.Linq;
using Toyota.TMHE.TPC.DAL.Linq;

namespace SecureChat.DAL.Linq
{
    public class SecureChatDataContext : SecureChatDataClassesDataContext, IDisposable
    {
        private SecureChatDataContext()
            : base(DbUtilities.GetConnectionString())
        {
            this._outerContext = _context_static;
            _context_static = this;
        }

        [ThreadStatic]
        private static SecureChatDataContext _context_static;
        private readonly SecureChatDataContext _outerContext;
        private int _useCount = 0;

        public static SecureChatDataContext Create()
        {
            return new SecureChatDataContext();
        }

        public static SecureChatDataContext CreateReuse()
        {
            if (_context_static == null)
                return SecureChatDataContext.Create();

            _context_static._useCount++;
            return _context_static;
        }

        public override void SubmitChanges(ConflictMode failureMode)
        {
            if (this._useCount == 0)
            {
                base.SubmitChanges(failureMode);
            }
        }

        new public void Dispose()
        {
            if (this._useCount > 0)
            {
                this._useCount--;
            }
            else
            {
                _context_static = this._outerContext;
                base.Dispose();
            }
        }
    }
}