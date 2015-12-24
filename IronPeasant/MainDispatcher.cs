using System;
using System.Drawing;
using System.Windows.Forms;
using IronPeasant.ViewSystem;

namespace IronPeasant
{
	internal class MainDispatcher : IDisposable
	{
		private readonly NotifyIcon notifyIcon;

		public MainDispatcher()
		{
			notifyIcon = new NotifyIcon();
			notifyIcon.Visible = true;
			notifyIcon.Icon = new Icon("iron.ico");

			var contextMenu = new ContextMenu();
			contextMenu.MenuItems.Add(new MenuItem("Настройки", NotifyIconPreferencesMenuClick));
			contextMenu.MenuItems.Add(new MenuItem("Об IronStorage", NotifyIconAboutBoxMenuClick));
			contextMenu.MenuItems.Add(new MenuItem("Выход", NotifyIconExitMenuClick));
			notifyIcon.ContextMenu = contextMenu;
		}

		public void Dispose()
		{
			notifyIcon.Dispose();
		}

		public void NotifyIconAboutBoxMenuClick(Object sender, EventArgs e)
		{
			Form aboutBox = new AboutBox();
			aboutBox.ShowDialog();
		}

		public void NotifyIconPreferencesMenuClick(Object sender, EventArgs e)
		{
			Form preferencesForm = new PreferencesForm();
			preferencesForm.ShowDialog();
		}

		public void NotifyIconExitMenuClick(Object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}