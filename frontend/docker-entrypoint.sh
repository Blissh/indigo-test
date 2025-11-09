#!/bin/sh
# Script para generar nginx.conf desde template con variables de entorno

# Valor por defecto si no se proporciona BACKEND_URL (para desarrollo local)
BACKEND_URL=${BACKEND_URL:-http://productsapi:5000}

# Asegurarse de que BACKEND_URL no termine con /
BACKEND_URL=$(echo "$BACKEND_URL" | sed 's:/*$::')

# Reemplazar la variable en el template
envsubst '${BACKEND_URL}' < /etc/nginx/templates/default.conf.template > /etc/nginx/conf.d/default.conf

# Iniciar nginx
exec nginx -g 'daemon off;'

