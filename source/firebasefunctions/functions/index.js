const functions = require('firebase-functions');
const admin = require('firebase-admin');
admin.initializeApp();


exports.addEntry = functions
    .region('asia-northeast1')
    .https.onCall((data, context) =>
    {
    const entry = {
        name : data.name,
        time : data.time,
        balloon : data.balloon,
        hit : data.hit,
        date : data.date
    };
    return admin.firestore().collection('entries')
        .add(entry)
        .then((snapshot) =>
    {
        return 'OK';
    });
});

exports.getTopEntries = functions
    .region('asia-northeast1')
    .https.onCall((data, context) =>
    {
    const count = data.count;

    return admin.firestore().collection('entries')
        .orderBy('name', 'desc')
        .limit(count)
        .get()
        .then((qSnapshot) =>
    {
        return {
        entries : qSnapshot.docs.map(x => x.data())
        };
    });
});