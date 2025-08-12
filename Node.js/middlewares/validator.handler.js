function validatorHandler(schema, property) {
    return (req, res, next) => {
        const data = req[property];
        const { error } = schema.validate(data, { abortEarly: true });
        if (error) {
            // Crear un error personalizado con detalles claros
            const customError = {
                statusCode: 400,
                error: 'Bad Request',
                message: error.details.map(detail => detail.message),
                details: error.details.map(detail => detail.message), // Mensajes claros de Joi
            };
            // Enviar el error al middleware de manejo de errores
            return res.status(400).send(customError);
        }
        //Si no hay error continuamos
        next();
    };
}

export default validatorHandler;