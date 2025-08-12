import joiHook from './verify.joi.hook.js';

export default function (fastify) {
    const onVerifyJoiHook = joiHook(fastify);

    fastify.addHook('preHandler', onVerifyJoiHook);
}