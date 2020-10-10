using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


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
        public Dictionary<T, int> counts;
        public int valueCount = 0;

        public Huffman()
        {
            counts = new Dictionary<T, int>();
            //var coladeprioridad = new Coladeprioridad<HuffmanNode<T>>();
        }

        public void fill(T s) {
            if (!counts.ContainsKey(s))
            {
                counts[s] = 0;
            }
            counts[s]++;
            valueCount++;
        }

        public String Huff() {
            var coladeprioridad = new Coladeprioridad<HuffmanNode<T>>();
            string palabras = "";
            string conocer = "";
            int repfrec = 1;
            foreach (T value in counts.Keys)
            {
                conocer = counts[value].ToString();
                if (conocer.Length > repfrec)
                {
                    repfrec = conocer.Length;
                }
            }

            byte[] Bytesdevalores = new byte[1];
            Bytesdevalores[0] = Convert.ToByte(counts.Count);
            var numvalores = Encoding.UTF8.GetString(Bytesdevalores);

            byte[] Bytesdecantidad = new byte[1];
            Bytesdecantidad[0] = Convert.ToByte(repfrec);
            var numcantidad = Encoding.UTF8.GetString(Bytesdecantidad);


            foreach (T value in counts.Keys)
            {

                palabras += value.ToString() + counts[value].ToString().PadLeft(repfrec, '0');
                var node = new HuffmanNode<T>((double)counts[value] / valueCount, value);
                coladeprioridad.Enqueue(node.Probability, node);
                _leafDictionary[value] = node;
            }
            conoceri = numvalores + numcantidad + palabras;

            while (coladeprioridad.contar > 1)
            {
                HuffmanNode<T> hijoizquieda = coladeprioridad.Dequeue();
                HuffmanNode<T> rightSon = coladeprioridad.Dequeue();
                var parent = new HuffmanNode<T>(hijoizquieda, rightSon);
                coladeprioridad.Enqueue(parent.Probability, parent);
            }

            _root = coladeprioridad.Dequeue();
            _root.IsZero = false;
            return conoceri;
        }

        public Huffman(IEnumerable<T> values)
        {
            var coladeprioridad = new Coladeprioridad<HuffmanNode<T>>();

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
            int repfrec = 1;
            foreach (T value in counts.Keys)
            {
                conocer = counts[value].ToString();
                if (conocer.Length > 1)
                {
                    repfrec = conocer.Length;
                }
            }

            byte[] Bytesdevalores = new byte[1];
            Bytesdevalores[0]=Convert.ToByte(counts.Count);
            var numvalores = Encoding.UTF8.GetString(Bytesdevalores);

            byte[] Bytesdecantidad = new byte[1];
            Bytesdecantidad[0] = Convert.ToByte(repfrec);
            var numcantidad = Encoding.UTF8.GetString(Bytesdecantidad);
            

            foreach (T value in counts.Keys)
            {

                palabras += value.ToString() + counts[value].ToString().PadLeft(repfrec, '0');
                var node = new HuffmanNode<T>((double)counts[value] / valueCount, value);
                coladeprioridad.Enqueue(node.Probability ,node);
                _leafDictionary[value] = node;
            }
            conoceri = numvalores + numcantidad + palabras;

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

        public List<T> Decodewometadata(string codigo) {
            codigo = Regresar(codigo);
            List<int> codigobin = new List<int>();
            foreach (var item in codigo.ToCharArray())
            {
                codigobin.Add(int.Parse(item.ToString()));
            }
            return Decode(codigobin);

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

        public void ArmarArbol(string codigo)
        {

            counts = new Dictionary<T, int>();
            var coladeprioridad = new Coladeprioridad<HuffmanNode<T>>();
            string variable = codigo.Substring(0,1);
            byte variables = Convert.ToByte(variable.ToCharArray()[0]);
            int final = Convert.ToInt32(variables);
            int cantvalores = Convert.ToInt32((Convert.ToByte(codigo.Substring(0,1).ToCharArray()[0])));
            int bytes = Convert.ToInt32((Convert.ToByte(codigo.Substring(1, 1).ToCharArray()[0]))) + 1;
            int frecuencia = 0;
            int probabilidad = 0;
            for (int i = 3; i < cantvalores*(bytes)+2; i += bytes)
            {
                frecuencia = int.Parse(codigo.Substring(i,bytes-1));
                probabilidad += frecuencia; }
            for (int i = 2; i < cantvalores*bytes+2; i += bytes)
            {
                string letra = codigo.Substring(i,1);
                frecuencia = int.Parse(codigo.Substring(i+1,bytes-1));
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
        }

        public string Regresar(string codigo) {
            int cantvalores = Convert.ToInt32(BitConverter.ToString(Encoding.UTF8.GetBytes(codigo.Substring(0, 1))));
            int bytes = Convert.ToInt32(BitConverter.ToString(Encoding.UTF8.GetBytes(codigo.Substring(1, 1)))) + 1;
            List<int> codigos = new List<int>();
            string aconvertir = codigo.Substring((cantvalores * bytes) + 2);
            List<byte> arrbytes = new List<byte>();
            String cadenabits = "";
            foreach (var item in aconvertir.ToCharArray())
            {
                arrbytes.Add(Convert.ToByte(item));
            }
            foreach (var item in arrbytes)
            {
                cadenabits += Convert.ToString(item,2).PadLeft(8,'0');
            }


            //yourByteString = Convert.ToString(valoresutf[0], 2).PadLeft(8, '0'); 

            return cadenabits;
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
                //if (returnValue.Count == maxvalores)
                //    break;
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
