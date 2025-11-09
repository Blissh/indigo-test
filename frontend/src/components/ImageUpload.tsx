import { useState } from 'react';
import { productService } from '../api/productService';
import { getErrorMessage } from '../utils/errorHandler';
import './ImageUpload.css';

interface ImageUploadProps {
  onImageUploaded: (url: string) => void;
  disabled?: boolean;
}

const ImageUpload = ({ onImageUploaded, disabled = false }: ImageUploadProps) => {
  const [uploading, setUploading] = useState(false);
  const [error, setError] = useState('');
  const [preview, setPreview] = useState<string | null>(null);

  const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    // Validación básica
    if (!file.type.startsWith('image/')) {
      setError('Por favor selecciona un archivo de imagen');
      return;
    }

    if (file.size > 5 * 1024 * 1024) {
      setError('El archivo es demasiado grande (máximo 5MB)');
      return;
    }

    setError('');
    setUploading(true);

    // Preview
    const reader = new FileReader();
    reader.onloadend = () => {
      setPreview(reader.result as string);
    };
    reader.readAsDataURL(file);

    try {
      const url = await productService.uploadImage(file);
      onImageUploaded(url);
    } catch (err) {
      setError(getErrorMessage(err));
      setPreview(null);
    } finally {
      setUploading(false);
    }
  };

  return (
    <div className="image-upload">
      <label className="upload-label" htmlFor="image-upload">
        {uploading ? 'Subiendo...' : 'Seleccionar Imagen'}
      </label>
      <input
        id="image-upload"
        type="file"
        accept="image/*"
        onChange={handleFileChange}
        disabled={disabled || uploading}
        className="file-input"
      />
      {preview && (
        <div className="image-preview">
          <img src={preview} alt="Preview" />
        </div>
      )}
      {error && <div className="upload-error">{error}</div>}
    </div>
  );
};

export default ImageUpload;

