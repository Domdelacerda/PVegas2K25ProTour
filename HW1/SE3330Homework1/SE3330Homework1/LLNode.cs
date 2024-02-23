using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE3330Homework1
{
    public class LLNode
    {
        private int data;
        private LLNode next;
        public LLNode(int data, LLNode next)
        {
            this.data = data;
            this.next = next;
        }
    }
}
