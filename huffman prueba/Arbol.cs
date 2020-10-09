using System;
using System.Collections;
using System.Collections.Generic;


namespace huffman_prueba
{
    internal class HuffmanNode<T> : IComparable
    {
        internal HuffmanNode(double probability, T value)
        {
            Probability = probability;
            LeftSon = RightSon = Parent = null;
            Value = value;
            IsLeaf = true;
        }

        internal HuffmanNode(HuffmanNode<T> leftSon, HuffmanNode<T> rightSon)
        {
            LeftSon = leftSon;
            RightSon = rightSon;
            Probability = leftSon.Probability + rightSon.Probability;
            leftSon.IsZero = true;
            rightSon.IsZero = false;
            leftSon.Parent = rightSon.Parent = this;
            IsLeaf = false;
        }

        internal HuffmanNode<T> LeftSon { get; set; }
        internal HuffmanNode<T> RightSon { get; set; }
        internal HuffmanNode<T> Parent { get; set; }
        internal T Value;
        internal bool IsLeaf { get; set; }

        internal bool IsZero { get; set; }

        internal int Bit
        {
            get { return IsZero ? 0 : 1; }
        }

        internal bool IsRoot
        {
            get { return Parent == null; }
        }

        internal double Probability { get; set; }

        public int CompareTo(object obj)
        {
            return -Probability.CompareTo(((HuffmanNode<T>)obj).Probability);
        }
    }

    public class Huffman<T> where T : IComparable
    {
        private readonly Dictionary<T, HuffmanNode<T>> _leafDictionary = new Dictionary<T, HuffmanNode<T>>();
        private  HuffmanNode<T> _root;
        public string conoceri = "";
        public Huffman(IEnumerable<T> values)
        {
            var counts = new Dictionary<T, int>();
            var coladeprioridad = new Coladeprioridad<HuffmanNode<T>>();
            int valueCount = 0;


            foreach (T value in values)
            {
                if (!counts.ContainsKey(value))
                {
                    counts[value] = 0;
                }
                counts[value]++;
                valueCount++;
            }
            string palabras = "";
            string conocer = "";
            string repfrec = "";

            foreach (T value in counts.Keys)
            {
               
                conocer = counts[value].ToString();
                if (conocer.Length > 1)
                {
                    for (int i = 0; i < conocer.Length -1; i++)
                    {
                        repfrec += "0";
                    }
                    palabras += value.ToString() + counts[value];
                    repfrec = "";

                }
                else
                {
                    palabras += value.ToString() + repfrec + counts[value];
                }
                conoceri = counts.Count.ToString() + ((conocer.Length)).ToString() + palabras;
                var node = new HuffmanNode<T>((double)counts[value] / valueCount, value);
                coladeprioridad.Enqueue(node.Probability ,node);
                _leafDictionary[value] = node;
            }

            while (coladeprioridad.contar > 1)
            {
                HuffmanNode<T> hijoizquieda = coladeprioridad.Dequeue();
                HuffmanNode<T> rightSon = coladeprioridad.Dequeue();
                var parent = new HuffmanNode<T>(hijoizquieda, rightSon);
                coladeprioridad.Enqueue(parent.Probability, parent);
            }

            _root = coladeprioridad.Dequeue();
            _root.IsZero = false;
        }

        public List<int> Encode(T value)
        {
            var returnValue = new List<int>();
            Encode(value, returnValue);
            return returnValue;
        }

        public void Encode(T value, List<int> encoding)
        {
            if (!_leafDictionary.ContainsKey(value))
            {
                throw new ArgumentException("Invalid value in Encode");
            }
            HuffmanNode<T> nodeCur = _leafDictionary[value];
            var reverseEncoding = new List<int>();
            while (!nodeCur.IsRoot)
            {
                reverseEncoding.Add(nodeCur.Bit);
                nodeCur = nodeCur.Parent;
            }

            reverseEncoding.Reverse();
            encoding.AddRange(reverseEncoding);
        }


        public List<int> Encode(IEnumerable<T> values)
        {
            var returnValue = new List<int>();

            foreach (T value in values)
            {
                Encode(value, returnValue);
            }
            return returnValue;
        }

        public T Decode(List<int> bitString, ref int position)
        {
            HuffmanNode<T> nodeCur = _root;
            while (!nodeCur.IsLeaf)
            {
                if (position > bitString.Count)
                {
                    throw new ArgumentException("Invalid bitstring in Decode");
                }
                nodeCur = bitString[position++] == 0 ? nodeCur.LeftSon : nodeCur.RightSon;
            }
            return nodeCur.Value;
        }

        public List<T> ArmarArbol(string codigo)
        {

            var counts = new Dictionary<T, int>();
            var coladeprioridad = new Coladeprioridad<HuffmanNode<T>>();
            int cantvalores = int.Parse(codigo.Substring(0, 1));
            int bytes = int.Parse(codigo.Substring(1,1)) + 1;
            int frecuencia = 0;
            int probabilidad = 0;
            for (int i = 3; i < cantvalores*2+2; i += 2)
            {
                frecuencia = int.Parse(codigo.Substring(i,1));
                probabilidad += frecuencia; }
            for (int i = 2; i < cantvalores*2+2; i += 2)
            {
                string letra = codigo.Substring(i,1);
                frecuencia = int.Parse(codigo.Substring(i+1,1));
                counts.Add(GetValue(letra), frecuencia);
                var node = new HuffmanNode<T>((double)frecuencia / probabilidad, GetValue(letra));
                coladeprioridad.Enqueue(node.Probability, node);
                _leafDictionary[GetValue(letra)] = node;
            }
            while (coladeprioridad.contar > 1)
            {
                HuffmanNode<T> hijoizquieda = coladeprioridad.Dequeue();
                HuffmanNode<T> rightSon = coladeprioridad.Dequeue();
                var parent = new HuffmanNode<T>(hijoizquieda, rightSon);
                coladeprioridad.Enqueue(parent.Probability, parent);
            }

            _root = coladeprioridad.Dequeue();
            List<int> codigos = new List<int>();
            for (int i = (cantvalores * 2)+2; i < codigo.Length; i++)
            {
                codigos.Add(int.Parse(codigo.Substring(i,1)));
            }
            List<T> palabra = new List<T>();
            
            return Decode(codigos);

        }
        public static T GetValue(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }


        public List<T> Decode(List<int> bitString)
        {
            int position = 0;
            var returnValue = new List<T>();

            while (position != bitString.Count)
            {
                returnValue.Add(Decode(bitString, ref position));
            }
            return returnValue;
        }

        public Byte[] GetBytesFromBinaryString(string binary)
        {
            var list = new List<Byte>();
            string h = "";
            for (int i = 0; i < binary.Length; i += 8)
            {
                if ((binary.Length-i)<8)
                {
                    h = binary.Substring(i, binary.Length - i);
                    h = h.PadRight(8,'0');
                    list.Add(Convert.ToByte(h, 2));
                }
                else
                {
                    string t = binary.Substring(i, 8);
                    list.Add(Convert.ToByte(t, 2));
                }

            }

            return list.ToArray();
        }
    }

}
