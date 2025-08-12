import prisma from './db.client.provider.js';
import bcrypt from 'bcrypt';

const userModel = {
  // Obtener un usuario con su info personal
  async findUnique(id) {
    const userId = parseInt(id, 10);
    return await prisma.user.findUnique({
      where: { id: userId },
      include: {
        userPersonalInfo: {
          select: {
            first_name:  true,
            second_name: true,
            phone_number:true,
            direction:   true,
            gender:      true
          }
        }
      }
    });
  },

  // Crear User + UserPersonalInfo en transacción
  async create(data, timestamp) {
    // separar datos de user y personalInfo
    const {
      username, email, password, observable = 'Show',
      first_name, second_name, phone_number, direction, gender
    } = data;

    const hashed = await bcrypt.hash(password, 10);

    return await prisma.$transaction(async (tx) => {
      const newUser = await tx.user.create({
        data: {
          username,
          email,
          password:    hashed,
          observable,
          role:        'User',
          created_at:  timestamp,
          updated_at:  timestamp,
          userPersonalInfo: {
            create: {
              first_name,
              second_name,
              phone_number,
              direction,
              gender,
              created_at: timestamp,
              updated_at: timestamp
            }
          }
        },
        include: { userPersonalInfo: true }
      });
      return newUser;
    });
  },

  // Listar usuarios con filtros y paginación
  async findMany(filters, page, limit) {
    const where = { role: 'User' };
    if (filters.search_name) {
      where.username = { startsWith: filters.search_name };
    }

    const total = await prisma.user.count({ where });

    const skip = (page - 1) * limit;
    const data = await prisma.user.findMany({
      where,
      skip,
      take: limit,
      select: {
        id:       true,
        username: true,
        email:    true,
        userPersonalInfo: {
          select: { first_name: true, second_name: true }
        }
      }
    });

    const totalPages = Math.ceil(total / limit);
    return { data, total, totalPages };
  },

  async update(id, data) {
    const userId = parseInt(id, 10);
    const {
      first_name, second_name, phone_number, direction, gender,
      ...userData
    } = data;

    if (userData.password) {
      userData.password = await bcrypt.hash(userData.password, 10);
    }

    // construir personalInfoData solo con campos definidos
    const personalInfoData = {};
    if (first_name    !== undefined) personalInfoData.first_name    = first_name;
    if (second_name   !== undefined) personalInfoData.second_name   = second_name;
    if (phone_number  !== undefined) personalInfoData.phone_number  = phone_number;
    if (direction     !== undefined) personalInfoData.direction     = direction;
    if (gender        !== undefined) personalInfoData.gender        = gender;
    //realizar transacción para insertar en varias tablas
    return await prisma.$transaction(async (tx) => {
      const updatedUser = await tx.user.update({
        where: { id: userId },
        data:  userData,
        include: { userPersonalInfo: true }
      });

      let updatedInfo = null;
      if (Object.keys(personalInfoData).length > 0) {
        updatedInfo = await tx.userPersonalInfo.update({
          where: { user_id: userId },
          data:  personalInfoData
        });
      }

      return { ...updatedUser, userPersonalInfo: updatedInfo ?? updatedUser.userPersonalInfo };
    });
  },
  // Eliminar usuario
  async delete(id) {
    const userId = parseInt(id, 10);
    return await prisma.user.delete({
      where: { id: userId }
    });
  }
};

export default userModel;
