﻿

namespace Difftaculous
{
    public interface IDiffPath
    {
        DiffPath Extend(string name);
        DiffPath Extend(int index);
    }



    public class DiffPath : IDiffPath
    {
        private static readonly DiffPath _root = FromJsonPath("$");


        private DiffPath()
        {
        }


        public static DiffPath Root { get { return _root; } }


        public static DiffPath FromJsonPath(string path)
        {
            return new DiffPath { AsJsonPath = path };
        }



        // TODO - we need a MUCH better way of indicating what is being extended
        public DiffPath Extend(string name)
        {
            return FromJsonPath(AsJsonPath + "." + name);
        }


        public DiffPath Extend(int index)
        {
            return FromJsonPath(AsJsonPath + "[" + index + "]");
        }



        // TODO - Such a horrible hack!
        public string AsJsonPath { get; private set; }


        public override bool Equals(object obj)
        {
            DiffPath other = obj as DiffPath;

            if (other == null)
            {
                return false;
            }

            return AsJsonPath == other.AsJsonPath;
        }


        public override int GetHashCode()
        {
            return AsJsonPath.GetHashCode();
        }


        public override string ToString()
        {
            return AsJsonPath;
        }
    }
}
