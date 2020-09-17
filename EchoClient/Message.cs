using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace EchoServer
{
    public class Message
    {
        private string message;
        private int amountOfASymbols;

        public Message()
        {
        }

        public Message(string messge)
        {
            this.message = messge;
            this.amountOfASymbols = messge.Length;
        }
        
        public string Serialize()
        {
            StreamWriter streamWriter = null;
            XmlSerializer xmlSerializer;
            string buffer;
            try
            {
                xmlSerializer = new XmlSerializer(typeof(Message));
                MemoryStream memStream = new MemoryStream();
                streamWriter = new StreamWriter(memStream);
                System.Xml.Serialization.XmlSerializerNamespaces xs = new XmlSerializerNamespaces();
                xs.Add("", "");
                xmlSerializer.Serialize(streamWriter, this, xs);
                buffer = Encoding.ASCII.GetString(memStream.GetBuffer());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(streamWriter != null)
                    streamWriter.Close();
            }

            return buffer;
        }

        public void DeSerialize(string xmlString)
        {
            XmlSerializer xmlSerializer;
            MemoryStream memStr = null;
            try
            {
                xmlSerializer = new XmlSerializer(this.GetType());
                byte[] bytes = new byte[xmlString.Length];
                Encoding.ASCII.GetBytes(xmlString, 0, xmlString.Length, bytes, 0);
                memStr = new MemoryStream(bytes);
                object objectFromXml = xmlSerializer.Deserialize(memStr);
                Message a = (Message) objectFromXml;
                this.Message1 = a.Message1;
                this.amountOfASymbols = a.amountOfASymbols;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (memStr != null)
                    memStr.Close();
            }
            
        }

        public string Message1
        {
            get => message;
            set => message = value;
        }

        public int AmountOfASymbols
        {
            get => amountOfASymbols;
            set => amountOfASymbols = value;
        }

        public override string ToString()
        {
            return message;
        }
    }
}