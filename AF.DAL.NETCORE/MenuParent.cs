using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AF.DAL
{
	public class MenuParent
	{
		public List<MenuChildren> LsMenuChildren { get; set; }
		public List<MenuParent> LsMenuParent { get; set; }
		public MenuParent()
		{
			DataTable dt = MenuDAO.GetMenus();
			foreach (DataRow dr in dt.Rows)
			{
				var htmlmenu = dr["OutPutText"];
				var idParent = dr["IdParent"];
			}
		}
	}
	public class MenuChildren
	{

	}
}
