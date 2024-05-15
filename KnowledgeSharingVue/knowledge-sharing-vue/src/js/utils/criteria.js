

class Criteria{
    static aggregateUsingMax(...criteria) {
        return Math.max(...criteria);
    }

    static aggregateUsingAverage(...criteria) {
        return criteria.reduce((total, current) => total + current, 0) / criteria.length;
    }
    
    static aggregateWithWeight(...criteria) {
        let product = 1.0;
        criteria.forEach(criterion => {
            product *= Math.pow(criterion, 0.5); // Use square root to speed up when the value is close to 1
        });
        return product;
    }
    
    static aggregateWithBoost(p, ...criteria) {
        let sum = 0.0;
        let boost = 0.0;
    
        criteria.forEach(criterion => {
            sum += criterion;
            boost += 1 - Math.pow(1 - criterion, p); // Use large exponent to speed up when the value is close to 1
        });
    
        let average = sum / criteria.length;
        let boostedAverage = (average + boost) / (1 + criteria.length); // Combine with offset factor
    
        return boostedAverage;
    }
    
    static maxCriteria(...criteria) {
        return Math.max(...criteria);
    }
    
    static exponentialCriteria(...criteria) {
        let sum = criteria.reduce((total, current) => total + current, 0);
        return 1 - Math.exp(-sum);
    }
    
    static yagerCriteria(p, ...criteria) {
        let sumOfPowers = criteria.map(a => Math.pow(1 - a, p)).reduce((total, current) => total + current, 0);
        return 1 - Math.min(1, Math.pow(sumOfPowers, 1 / p));
    }
}

export default Criteria;

