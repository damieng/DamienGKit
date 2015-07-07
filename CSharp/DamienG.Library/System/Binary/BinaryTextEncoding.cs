namespace DamienG.System.Binary
{
    public abstract class BinaryTextEncoding
    {
        public abstract string Encode(byte[] bytes);

        public abstract byte[] Decode(string input);
    }
}