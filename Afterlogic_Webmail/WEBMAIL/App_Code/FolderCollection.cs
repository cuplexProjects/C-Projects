using System;
using System.Collections;
using System.Globalization;

namespace WebMail
{
	/// <summary>
    /// This class provides properties and methods for managing folders.
	/// </summary>
	public class FolderCollection : CollectionBase
	{
		public FolderCollection() {}

		public Folder this[int index]
		{
			get { return (Folder) List[index]; }
			set { List[index] = value; }
		}

		public Folder this[string fullName]
		{
			get
			{
				foreach (Folder fld in List)
				{
					if (string.Compare(fld.FullPath, fullName, true, CultureInfo.InvariantCulture) == 0)
					{
						return fld;
					}
				}
				return null;
			}
		}

		public Folder this[FolderType type]
		{
			get
			{
				foreach (Folder fld in List)
				{
					if (fld.Type == type)
					{
						return fld;
					}

                    if (fld.Type == FolderType.Inbox && fld.SubFolders.Count > 0)
                    {
                        Folder tmp = fld.SubFolders[type];
                        if (tmp != null) return tmp;
                    }
				}
				return null;
			}
		}

		public int Add(Folder fld)
		{
			return List.Add(fld);
		}

		public int IndexOf(Folder fld)
		{
			return List.IndexOf(fld);
		}

		public void Insert(int index, Folder fld)
		{
			List.Insert(index, fld);
		}

		public void Remove(Folder fld)
		{
			List.Remove(fld);
		}

		public bool Contains(Folder fld)
		{
			return List.Contains(fld);
		}

		public static void Sort(FolderCollection foldersList)
		{
			Sort(foldersList, new FolderOrderer());
		}

		public static void Sort(FolderCollection foldersList, IComparer comparer)
		{
			for (int i = 0; i < foldersList.Count; i++)
			{
				for (int j = 0; j < foldersList.Count; j++)
				{
					int compareResult = foldersList[j].CompareTo(foldersList[i]);
					if (compareResult > 0)
					{
						Folder temp = foldersList[j];
						foldersList[j] = foldersList[i];
						foldersList[i] = temp;
					}
				}
			}
		}

		public static void SortTree(FolderCollection foldersTree)
		{
			SortTree(foldersTree, new FolderOrderer());
		}

		public static void SortTree(FolderCollection foldersTree, IComparer comparer)
		{
			Sort(foldersTree, comparer);
			foreach (Folder fld in foldersTree)
			{
				if ((fld.SubFolders != null)&&(fld.SubFolders.Count > 0))
				{
					SortTree(fld.SubFolders, comparer);
				}
			}
		}

		public Folder GetFolderByID(int id_folder)
		{
			foreach (Folder fld in List)
			{
				if (fld.ID == id_folder) return fld;
			}
			return null;
		}

		public static void CreateFolderListFromTree(ref FolderCollection resultList, FolderCollection folderTree)
		{
			if (folderTree != null)
			{
				foreach (Folder f in folderTree)
				{
					resultList.Add(f);
					if (f.SubFolders.Count > 0)
					{
						CreateFolderListFromTree(ref resultList, f.SubFolders);
					}
				}
			}
		}

        public static FolderCollection CreateImapFolderTreeFromList(MailBee.ImapMail.FolderCollection folderList,
            MailBee.ImapMail.FolderCollection subscribedFolderList)
        {
            FolderCollection result = new FolderCollection();

            if (folderList != null)
            {
                int nestingLevel = 0;
                int addedCount = 0;
                while (folderList.Count > addedCount)
                {
                    for (int i = 0; i < folderList.Count; i++)
                    {
                        MailBee.ImapMail.Folder fld = folderList[i];
                        if (fld.NestingLevel == nestingLevel)
                        {
                            bool hide = true;
                            int subsCount = subscribedFolderList.Count;
                            for (int j = 0; j < subsCount; j++)
                            {
                                if (fld.Name == subscribedFolderList[j].Name)
                                {
                                    hide = false;
                                    subscribedFolderList.RemoveAt(j);
                                    subsCount--;
                                    break;
                                }
                            }
                            if (nestingLevel == 0)
                            {
                                result.Add(new Folder(fld, hide));
                            }
                            else
                            {
                                foreach (Folder resultFolder in result)
                                {
                                    if (string.Compare(string.Format("{0}{1}{2}", resultFolder.ImapFolder.Name, resultFolder.ImapFolder.Delimiter, fld.ShortName), fld.Name, true, CultureInfo.InvariantCulture) == 0)
                                    {
                                        resultFolder.SubFolders.Add(new Folder(fld, hide));
                                        break;
                                    }

                                    foreach (Folder resultFolder2 in resultFolder.SubFolders)
                                    {
                                        if (string.Compare(string.Format("{0}{1}{2}", resultFolder2.ImapFolder.Name, resultFolder2.ImapFolder.Delimiter, fld.ShortName), fld.Name, true, CultureInfo.InvariantCulture) == 0)
                                        {
                                            resultFolder2.SubFolders.Add(new Folder(fld, hide));
                                            break;
                                        }

                                        foreach (Folder resultFolder3 in resultFolder2.SubFolders)
                                        {
                                            if (string.Compare(string.Format("{0}{1}{2}", resultFolder3.ImapFolder.Name, resultFolder3.ImapFolder.Delimiter, fld.ShortName), fld.Name, true, CultureInfo.InvariantCulture) == 0)
                                            {
                                                resultFolder3.SubFolders.Add(new Folder(fld, hide));
                                                break;
                                            }

                                            foreach (Folder resultFolder4 in resultFolder3.SubFolders)
                                            {
                                                if (string.Compare(string.Format("{0}{1}{2}", resultFolder4.ImapFolder.Name, resultFolder4.ImapFolder.Delimiter, fld.ShortName), fld.Name, true, CultureInfo.InvariantCulture) == 0)
                                                {
                                                    resultFolder4.SubFolders.Add(new Folder(fld, hide));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            addedCount++;
                        }
                    }
                    nestingLevel++;
                }
            }

            return result;
        }

		public static FolderCollection CreateDatabaseFolderTreeFromList(FolderCollection folderList)
		{
			FolderCollection result = new FolderCollection();

			if (folderList != null)
			{
				foreach (Folder listFolder in folderList)
				{
					if (!AddDatabaseFolderNodeToTree(listFolder, result))
					{
						result.Add(listFolder);
					}
				}
			}

			return result;
		}

		protected static bool AddDatabaseFolderNodeToTree(Folder fld, FolderCollection folderList)
		{
			if ((folderList != null) && (fld != null))
			{
				foreach (Folder listFolder in folderList)
				{
					if (listFolder.ID == fld.IDParent)
					{
						listFolder.SubFolders.Add(fld);
						return true;
					}
					bool added = AddDatabaseFolderNodeToTree(fld, listFolder.SubFolders);
					if (added) return true;
				}
			}
			return false;
		}

        public void ReSetSyncTypeToDirectMode()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].SyncType = FolderSyncType.DirectMode;
                if (this[i].SubFolders.Count > 0)
                {
                    this[i].SubFolders.ReSetSyncTypeToDirectMode();
                }
            }
        }
    }

    public class FolderOrderer : IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
			if (x == y) return 0;
			if (x == null) return -1;
			if (y == null) return 1;
			IComparable ix = x as IComparable;
			if (ix != null)
				return ix.CompareTo(y);
			IComparable iy = y as IComparable;
			if (iy != null)
				return -iy.CompareTo(x);
			throw new ArgumentException("Folders comparer error");
		}

		#endregion

	}

}
