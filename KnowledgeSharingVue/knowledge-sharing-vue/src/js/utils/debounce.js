

class Debounce{
    /**
     * This function will return a debounced function that will only call the original function after the delay has passed.
     * @param {*} callback - The function to call after the delay has passed.
     * @param {*} delay - The number of milliseconds to wait before calling the function.
     * @param {*} options - An object that can contain the following properties: leading: boolean, trailing: boolean
     * @returns {Function} - A debounced function that will only call the original function after the delay has passed.
     * @example let debouncedFunction = Debounce.debounce(() => console.log('Hello World'), 1000, {leading: true, trailing: true});
     * @Created PhuctTV (17/5/24)
     * @Modified None
     */
    static debounce(callback, delay, options = {}){
        let timeoutId = null;
        const {leading = true, trailing = true} = options;

        return function(...args) {
            const context = this;
            
            if (leading && timeoutId == null) {
                callback.apply(context, args);
            }

            const invokeCallback = () => {
                timeoutId = null;
                if (trailing) {
                    callback.apply(context, args);
                }
            };

            clearTimeout(timeoutId);
            timeoutId = setTimeout(invokeCallback, delay);
        };
    }    


    /**
     * This function will return a throttled function that will only call the original function at most once every deltaTime milliseconds.
     * @param {*} callback - The function to call at most once every deltaTime milliseconds.
     * @param {*} deltaTime - The number of milliseconds to wait before calling the function.
     * @returns {Function} - A throttled function that will only call the original function at most once every deltaTime milliseconds.
     * @example let throttledFunction = Debounce.throttle(() => console.log('Hello World'), 1000);
     * @Created PhuctTV (17/5/24)
     * @Modified None
     */
    static throttle(callback, deltaTime) {
        let shouldWait = false;
        let waitingArgs = null;
        
        const timeoutFunc = () => {
            if (waitingArgs == null) {
                shouldWait = false;
            } else {
                callback.apply(this, waitingArgs);
                waitingArgs = null;
                setTimeout(timeoutFunc, deltaTime);
            }
        };
        
        return function(...args) {
            if (shouldWait) {
                waitingArgs = args;
                return;
            }
        
            callback.apply(this, args);
            shouldWait = true;
        
            setTimeout(timeoutFunc, deltaTime);
        };
    }
}

export default Debounce;
