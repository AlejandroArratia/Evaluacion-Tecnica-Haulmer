# Evaluación Técnica - Instrucciones

## Inicialización del Programa

1. **Descargar Repositorio:**
   - Clonar o descargar el repositorio "Evaluacion Tecnica".

2. **Abrir Carpeta del Código Fuente:**
   - Abrir la carpeta "Codigo Fuente" en tu editor de código.

3. **Ejecutar el Programa:**
   - Asegurarse de que el programa esté ejecutándose mediante "TodoApi.csproj".
   - Si no se está ejecutando, escribir en la terminal:
     ```bash
     dotnet run
     ```

## Pruebas con Postman

4. **Importar Colección de Pruebas:**
   - Abrir Postman e importar la colección "Collection_test.postman_collection.json".

5. **Ejecutar Pruebas:**
   - Ejecutar las siguientes pruebas de la colección:
     - **User-Post:** Agrega un usuario.
     - **User-Get:** Lista todos los usuarios ingresados.
     - **Encrypt:** Encripta texto usando lógica AES.
     - **Decrypt:** Desencripta texto mediante la misma lógica anterior.

## Suposiciones

- Se utiliza una base de datos en memoria local para modo de prueba.
- La fecha de creación se agrega automáticamente al momento de crear el usuario.
- La fecha de actualización se indica como NULL y se modifica solo mediante un PUT.
- Se utiliza el algoritmo AES para la encriptación y está "hardcoded" utilizando una clave fija.
- Se asume que el entorno de ejecución soporta ASP.NET Core y Entity Framework Core.

## Video de Prueba

[![Ver el video](https://img.youtube.com/vi/BN3zMdFRzIs/hqdefault.jpg)](https://www.youtube.com/embed/BN3zMdFRzIs)
