using System.Xml.Linq;

namespace SE3330Homework1
{
    public class LList : IDisposable
    {
        private LLNode head;
        public LList(LLNode head)
        {
            this.head = head;
        }

        public LLNode GetHead()
        {
            return head;
        }

        public bool isEmpty()
        {
            return head == null;
        }

        // Andrew's Code
        public int[] displayList()

        {

            Console.WriteLine();

            LLNode node1 = GetHead();

            int x = 0;

            if (isEmpty())
            {

                Console.Write("No Data");

                return null;

            }

            while (node1 != null)

            {

                x++;

                node1 = node1.getNext();

            }

            node1 = GetHead();

            int[] ints = new int[x];

            x = 0;

            while (node1 != null)
            {

                ints[x] = node1.getData();

                Console.Write("Data " + x + ": " + ints[x] + "\t");

                x++;

                node1 = node1.getNext();

            }

            return ints;

        }

        public int Length()
        {
            int length = 0;

            LLNode current = GetHead();

            while(current != null)
            {
                length++;
                current = current.getNext();
            }
             
            return length;
        }

        public void DeleteList()
        {
            Dispose();
        }

        public void Dispose()
        {
            LLNode temp;
            LLNode head = GetHead();

            while (head != null)
            {
                temp = head;
                head = head.getNext();
                temp = null;

            }

        }
    }
}