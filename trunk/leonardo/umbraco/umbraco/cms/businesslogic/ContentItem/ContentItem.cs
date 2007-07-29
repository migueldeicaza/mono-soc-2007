using System;

namespace Umbraco.Cms.BusinessLogic.contentitem
{
	/// <summary>
	/// Summary description for ContentItem.
	/// </summary>
	public class ContentItem : Content
	{
		private static Guid _objectType = new Guid("10e2b09f-c28b-476d-b77a-aa686435e44a");

		public ContentItem(int id) : base(id)
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public ContentItem(Guid id) : base(id)
		{
			//
			// TODO: Add constructor logic here
			//
		}


        /// <summary>
        /// Used to persist object changes to the database. In v3.0 it's just a stub for future compatibility
        /// </summary>

        public override void Save()
        {
            base.Save();
        }



		public static void DeleteFromType(ContentItemType cit) 
		{
			foreach (Content c in Content.getContentOfContentType(cit)) 
			{
				// due to recursive structure document might already been deleted..
				if (CMSNode.IsNode(c.UniqueId)) 
				{
					ContentItem tmp = new ContentItem(c.UniqueId);
					tmp.delete();
				}
			}
		}

		public static ContentItem MakeNew(string Name, ContentItemType cit, BusinessLogic.User u, int ParentId) 
		{			
			Guid newId = Guid.NewGuid();
			// Updated to match level from base node
			CMSNode n = new CMSNode(ParentId);
			int newLevel = n.Level;
			newLevel++;
			CMSNode.MakeNew(ParentId,_objectType, u.Id, newLevel,  Name, newId);
			ContentItem tmp = new ContentItem(newId);
			tmp.CreateContent(cit);
			return tmp;
		}

		
		new public void delete() 
		{
			foreach (ContentItem d in this.Children) 
			{
				d.delete();
			}
			base.delete();
		}
		
		new public ContentItem[] Children 
		{
			get
			{
				BusinessLogic.console.IconI[] tmp = base.Children;
				ContentItem[] retval = new ContentItem[tmp.Length];
				for (int i = 0; i < tmp.Length; i++) retval[i] = new ContentItem(tmp[i].UniqueId);
				return retval;
			}
		}
	}
}
