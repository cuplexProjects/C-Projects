using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Linq.Expressions;

namespace CuplexLib.Linq
{
    public class DataContext : CuplexDataClassesDataContext, IDisposable
    {
        private DataContext()
            : base(CuplexLib.Utils.GetConnectionString())
        {
            _outerContext = _context_static;
            _context_static = this;
        }

        [ThreadStatic]
        private static DataContext _context_static = null;
        private DataContext _outerContext = null;
        private int _useCount = 0;

        public static DataContext Create()
        {
            return new DataContext();
        }

        public static DataContext CreateReuse()
        {
            if (_context_static == null)
                return DataContext.Create();

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
                _context_static = _outerContext;
                base.Dispose();
            }
        }
    }
}

namespace CuplexLib.LinqExtensions
{

    public static class TpLoadOptionsHelper
    {
        // usage example: 
        //	db.LoadOptions = new DataLoadOptions().
        //		TpLoadWith<CostDivision>(cd => cd.Color).
        //		TpLoadWith<CostDivision>(cd => cd.Hotel);
        public static DataLoadOptions TpLoadWith<T>(this DataLoadOptions loadOptions, Expression<Func<T, Object>> expression)
        {
            loadOptions.LoadWith<T>(expression);
            return loadOptions;
        }
        public static DataLoadOptions TpAssociateWith<T>(this DataLoadOptions loadOptions, Expression<Func<T, Object>> expression)
        {
            loadOptions.AssociateWith<T>(expression);
            return loadOptions;
        }
    }


    // from http://www.albahari.com/nutshell/predicatebuilder.aspx
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
