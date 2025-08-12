import userModel from './user.model.js';
import { DateTime } from 'luxon';

class UserController {
  async getUserById(id) {
    return await userModel.findUnique(id);
  }

  async createUser(data) {
    const now = DateTime.utc().toISO();
    return await userModel.create(data, now);
  }

  async getAllUsers(filters, page, limit) {
    return await userModel.findMany(filters, page, limit);
  }

  async updateUser(id, data) {
    return await userModel.update(id, data);
  }

  async deleteUser(id) {
    return await userModel.delete(id);
  }
}

export default new UserController();
