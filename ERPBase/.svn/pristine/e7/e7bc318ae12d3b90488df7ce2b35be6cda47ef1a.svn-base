using System;
using System.Web.Mvc;

using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Text;
namespace HaloUIHelpers.Helpers
{
    public class JUIBox : IDisposable
    {
         private bool _disposed;
        private readonly ViewContext _viewContext;

        public JUIBox(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }
            _viewContext = viewContext;
        }
        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                JUIBoxExtension.EndJUIBox(_viewContext);
            }
        }
        public void EndJUIBox()
        {
            Dispose(true);
        }

    }
}