using System.Collections;

namespace FileSystemImage.FileTree
{
    public class FileSystemTree
    {
        //Storage for all tree nodes
        private readonly FileAlocationTable _fileAlocationTable;

        // Root node created on init
        private readonly TreeNode root;

        public FileSystemTree()
        {
            this._fileAlocationTable = new FileAlocationTable();
            this.root = new TreeNode(false, "Root", 0, 0);
        }

        public TreeNode Root
        {
            get { return this.root; }
        }

        private FileAlocationTable FileRecords
        {
            get { return this._fileAlocationTable; }
        }

        public byte[] ToArray()
        {
            byte[] result = new byte[10];

            return result;
        }

        public static FileSystemTree RestoreInstance(byte[] data)
        {
            FileSystemTree fileSystemTree = new FileSystemTree();
            return fileSystemTree;
        }
    }

    public class FileAlocationTable
    {
        private long cursor;

        public FileAlocationTable()
        {
            this.FileRecords = new Hashtable();
        }

        public Hashtable FileRecords { get; private set; }

        public long AddTreeNode(TreeNode node)
        {
            this.FileRecords.Add(this.cursor, node);
            return this.cursor++;
        }

        public void RemoveTreeNode(long key)
        {
            this.FileRecords.Remove(key);
        }
    }
}