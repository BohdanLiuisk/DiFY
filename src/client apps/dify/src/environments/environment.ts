export const environment = {
  production: false,
  coreUrl: 'http://localhost:5050',
  authConfig: {
    client_id: 'ro.client',
    client_secret: 'secret'
  },
  peerOptions: {
    key: 'peerjs',
    host: 'localhost',
    port: 3000,
    secure: true,
    path: '/',
    debug: 1,
    config: {
      iceServers: [
        { urls: 'stun:stun1.l.google.com:19302' },
        { urls: 'stun:stun2.l.google.com:19302' }
      ],
    },
  }
};
