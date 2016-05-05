var EventStoreClient = require('event-store-client');

var connection = new EventStoreClient.Connection({
    host: 'localhost',
    port: '1113',
    debug: false,
    onConnect() {
        console.log('onConnect', arguments)
    },
    onError() {
        console.log('onError', arguments)
    }
});

var stories = {};

connection.readAllEventsForward(
    0,
    0,
    1000,
    true,
    true,
    function onEventAppeared(eventData) {
        if (!eventData.streamId.startsWith('aggregate-')) {
            return;
        }
        if (eventData.eventType === 'StoryBirthdayEvent') {
            stories[eventData.streamId] = {id: eventData.data.id, assigneesCount: 0};
            console.log('new story was born', stories[eventData.streamId]);
        } else if (eventData.eventType === 'PersonAssignedEvent') {
            stories[eventData.streamId].assigneesCount++;
            console.log('person assigned', stories[eventData.streamId]);
        } else {
            console.log('unknown evt', JSON.stringify(eventData, null, 4));
        }
    },
    {username: 'admin', password: 'changeit'},
    function () {
        console.log('started');
    }
);
