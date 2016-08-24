using System;
using System.Data.Linq;

namespace DatabaseLib.Linq
{
    public class ApiDataContext : DBDataClassesDataContext, IDisposable
    {
        private ApiDataContext()
            : base(DBHelper.GetConnectionString())
        {
            this._outerContext = _context_static;
            _context_static = this;
        }

        [ThreadStatic]
        private static ApiDataContext _context_static;
        private readonly ApiDataContext _outerContext;
        private int _useCount = 0;

        public static ApiDataContext Create()
        {
            return new ApiDataContext();
        }

        public static ApiDataContext CreateReuse()
        {
            if (_context_static == null)
                return ApiDataContext.Create();

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