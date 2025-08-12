class ResponseStatus{
    StatusOK(reply, status, message, data = []){
        return reply.status(status).send({
            success: true,
            message: message,
            data: data
        });
    }
    StatusPagination(reply, status, message, paginationInfo, data = []){
        return reply.status(status).send({
            success: true,
            message: message,
            ...paginationInfo,
            data: data
        });
    }
    StatusError(reply, status, custom_msg, error, message){
        return reply.status(status).send({
            custom_msg: custom_msg,
            error: error,
            message: message
        });
    }
    //Gestor de errores
    SetErrorHandler(error, reply){
        let custom_msg;

        if (databaseErrorCodes.includes(error.code)) {
            if(error.code === 'P2002'){
                custom_msg = 'Violación de clave única en la base de datos'
                error.message = this.UniqueConstraintErrorHandler(error);
            }else{
                custom_msg = 'Error en la base de datos';
            }
        }else{
            // Manejo de errores generales
            custom_msg = 'Internal Server Error';
        }
        this.StatusError(reply, 500, custom_msg, error.name, error.message);
    }
}

export default new ResponseStatus;