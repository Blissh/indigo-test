#!/bin/sh
# Script para generar nginx.conf desde template con variables de entorno

# Valor por defecto si no se proporciona BACKEND_URL (para desarrollo local)
BACKEND_URL=${BACKEND_URL:-http://productsapi:5000}

# Si BACKEND_URL no tiene protocolo (solo hostname), agregar https://
# Esto es necesario cuando Render usa property: host en lugar de property: url
if echo "$BACKEND_URL" | grep -qvE '^https?://'; then
    BACKEND_URL="https://${BACKEND_URL}"
fi

# Asegurarse de que BACKEND_URL no termine con /
BACKEND_URL=$(echo "$BACKEND_URL" | sed 's:/*$::')

# Validar que BACKEND_URL esté configurada
if [ -z "$BACKEND_URL" ]; then
    echo "ERROR: BACKEND_URL no está configurada"
    exit 1
fi

echo "Configurando nginx con BACKEND_URL: $BACKEND_URL"

# Reemplazar la variable en el template
envsubst '${BACKEND_URL}' < /etc/nginx/templates/default.conf.template > /etc/nginx/conf.d/default.conf

# Validar la configuración de nginx antes de iniciar
nginx -t || {
    echo "ERROR: Configuración de nginx inválida"
    cat /etc/nginx/conf.d/default.conf
    exit 1
}

# Iniciar nginx
exec nginx -g 'daemon off;'

