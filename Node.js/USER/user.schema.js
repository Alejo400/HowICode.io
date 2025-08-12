import Joi from 'joi';

const id = Joi.number().integer().min(1).messages({
  'number.base':      '"id" debe ser un número',
  'number.integer':   '"id" debe ser un entero',
  'number.min':       '"id" debe ser al menos {#limit}'
});

const username = Joi.string().min(3).max(50).messages({
  'string.base':      '"username" debe ser una cadena de texto',
  'string.min':       '"username" debe tener al menos {#limit} caracteres',
  'string.max':       '"username" debe tener como máximo {#limit} caracteres'
});

const password = Joi.string().min(6).max(255).messages({
  'string.base':      '"password" debe ser una cadena de texto',
  'string.min':       '"password" debe tener al menos {#limit} caracteres',
  'string.max':       '"password" debe tener como máximo {#limit} caracteres'
});

const email = Joi.string().email().messages({
  'string.base':      '"email" debe ser una cadena de texto',
  'string.email':     '"email" debe ser un correo electrónico válido'
});

const search_name = Joi.string().min(1).max(100).messages({
  'string.base':      '"search_name" debe ser una cadena de texto',
  'string.min':       '"search_name" debe tener al menos {#limit} caracteres',
  'string.max':       '"search_name" debe tener como máximo {#limit} caracteres'
});

const page = Joi.number().integer().min(1).optional().messages({
  'number.base':      '"page" debe ser un número',
  'number.integer':   '"page" debe ser un entero',
  'number.min':       '"page" debe ser al menos {#limit}'
});

const limit = Joi.number().integer().min(1).optional().messages({
  'number.base':      '"limit" debe ser un número',
  'number.integer':   '"limit" debe ser un entero',
  'number.min':       '"limit" debe ser al menos {#limit}'
});

const first_name = Joi.string().min(3).max(50).messages({
  'string.base':      '"first_name" debe ser una cadena de texto',
  'string.min':       '"first_name" debe tener al menos {#limit} caracteres',
  'string.max':       '"first_name" debe tener como máximo {#limit} caracteres'
});

const second_name = Joi.string().min(3).max(50).optional().messages({
  'string.base':      '"second_name" debe ser una cadena de texto',
  'string.min':       '"second_name" debe tener al menos {#limit} caracteres',
  'string.max':       '"second_name" debe tener como máximo {#limit} caracteres'
});

const phone_number = Joi.string().min(6).max(20).messages({
  'string.base':      '"phone_number" debe ser una cadena de texto',
  'string.min':       '"phone_number" debe tener al menos {#limit} caracteres',
  'string.max':       '"phone_number" debe tener como máximo {#limit} caracteres'
});

const direction = Joi.string().min(6).max(255).messages({
  'string.base':      '"direction" debe ser una cadena de texto',
  'string.min':       '"direction" debe tener al menos {#limit} caracteres',
  'string.max':       '"direction" debe tener como máximo {#limit} caracteres'
});

const gender = Joi.string().valid('M','F').messages({
  'string.base':      '"gender" debe ser una cadena de texto',
  'any.only':         '"gender" debe ser "M" o "F"'
});

const userSchema = {
  // GET /:id and DELETE /:id
  getUserSchema: Joi.object({
    id: id.required()
  }),

  // POST /login
  loginUserSchema: Joi.object({
    username: username.required(),
    password: password.required()
  }),

  // POST /
  createUserSchema: Joi.object({
    username:    username.required(),
    email:       email.required(),
    password:    password.required(),
    first_name:  first_name.required(),
    second_name: second_name,
    phone_number:phone_number.required(),
    direction:   direction.required(),
    gender:      gender.required()
  }),

  // GET /GetAllUsers?search_name=&page=&limit=
  getAllUsersQuerySchema: Joi.object({
    search_name,
    page,
    limit
  }),

  // PATCH /:id
  updateUserSchema: Joi.object({
    id: id.required()
  }),
  updateUserBodySchema: Joi.object({
    username,
    email,
    password,
    first_name,
    second_name,
    phone_number,
    direction,
    gender
  }).min(1), // al menos un campo

  // DELETE /:id
  deleteUserSchema: Joi.object({
    id: id.required()
  })
};

export default userSchema;
