namespace Utils {
    public class Size {
        private int _width;
        private int _height;

        public int Width {
            get => _width;
            set => _width = value;
        }

        public int Height {
            get => _height;
            set => _height = value;
        }

        public Size(int width, int height) {
            _width = width;
            _height = height;
        }
    }
}