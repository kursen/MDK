using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaloUIHelpers.Helpers
{
    public class MvcPanel:IDisposable
    {
        // Fields
        private bool _disposed;
        private readonly ViewContext _viewContext;

        public MvcPanel(ViewContext viewContext)
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
                MvcPanelExtension.EndPanel(_viewContext);
            }
        }
        public void EndPanel()
        {
            Dispose(true);
        }

        public enum PanelType
        {
            panelDefault,
            panelPrimary,
            panelInfo,
            panelWarning,
            panelDanger,
            panelSucces

        };
        
    }
}