import userController from './user.controller.js';
import validatorHandler from '../middlewares/validator.handler.js';
import userSchema from './user.schema.js';
import ResponseStatus from '../Utilies/Validations/responseStatus.js';
import { checkRoles } from '../middlewares/auth.handler.js';

const userRoutes = async (fastify) => {
  // Obtener un usuario por ID
  fastify.get('/:id', {
    preHandler: [
      checkRoles('Admin', 'User'),
      validatorHandler(userSchema.getUserSchema, 'params')
    ],
    handler: async (req, reply) => {
      try {
        const user = await userController.getUserById(req.params.id);
        const message = user
          ? 'Usuario encontrado'
          : 'Usuario no existe';
        const status  = user ? 200 : 404;
        ResponseStatus.StatusOK(reply, status, message, user);
      } catch (error) {
        ResponseStatus.SetErrorHandler(error, reply);
      }
    },
  });

  // Crear un nuevo usuario (con UserPersonalInfo)  
  fastify.post('/', {
    preHandler: [
      checkRoles('Admin'),
      validatorHandler(userSchema.createUserSchema, 'body')
    ],
    handler: async (req, reply) => {
      try {
        const result = await userController.createUser(req.body);
        ResponseStatus.StatusOK(reply, 201, 'Usuario creado con éxito', result);
      } catch (error) {
        ResponseStatus.SetErrorHandler(error, reply);
      }
    },
  });

  // Obtener todos los usuarios con filtros + paginación
  fastify.get('/GetAllUsers', {
    preHandler: [
      checkRoles('Admin'),
      validatorHandler(userSchema.getAllUsersQuerySchema, 'query')
    ],
    handler: async (req, reply) => {
      try {
        const page  = parseInt(req.query.page, 10)  || 1;
        const limit = parseInt(req.query.limit, 10) || 50;
        const { page: _, limit: __, ...filters } = req.query;

        const { data, total, totalPages } =
          await userController.getAllUsers(filters, page, limit);

        const qs = new URLSearchParams(filters).toString();
        const basePath = `${process.env.URL_BACKEND}user/GetAllUsers`;
        const makeUrl = p => `${basePath}?${qs}${qs ? '&' : ''}page=${p}&limit=${limit}`;

        const next     = page < totalPages ? makeUrl(page + 1) : null;
        const prev     = page > 1          ? makeUrl(page - 1) : null;
        const count    = data.length;

        const paginationInfo = { total, count, totalPages, next, prev };

        const message = data.length
          ? 'Usuarios encontrados'
          : 'No hay usuarios registrados';

        ResponseStatus.StatusPagination(reply, 200, message, paginationInfo, data);
      } catch (error) {
        ResponseStatus.SetErrorHandler(error, reply);
      }
    },
  });

  // Actualizar un usuario (PATCH parcial)
  fastify.patch('/:id', {
    preHandler: [
      checkRoles('Admin', 'User'),
      validatorHandler(userSchema.updateUserSchema, 'params'),
      validatorHandler(userSchema.updateUserBodySchema, 'body')
    ],
    handler: async (req, reply) => {
      try {
        const result = await userController.updateUser(req.params.id, req.body);
        ResponseStatus.StatusOK(reply, 200, 'Usuario actualizado', result);
      } catch (error) {
        ResponseStatus.SetErrorHandler(error, reply);
      }
    },
  });

  // Eliminar un usuario
  fastify.delete('/:id', {
    preHandler: [
      checkRoles('Admin'),
      validatorHandler(userSchema.getUserSchema, 'params')
    ],
    handler: async (req, reply) => {
      try {
        const result = await userController.deleteUser(req.params.id);
        ResponseStatus.StatusOK(reply, 200, 'Usuario eliminado', result);
      } catch (error) {
        ResponseStatus.SetErrorHandler(error, reply);
      }
    },
  });
};

export default userRoutes;
